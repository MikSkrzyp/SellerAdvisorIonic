<script setup>
import { ref, onMounted } from 'vue';
import { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonFab, IonFabButton, IonIcon, alertController } from '@ionic/vue';
import { BarcodeScanner } from '@capacitor-mlkit/barcode-scanning';
import { scan } from 'ionicons/icons';
import { useRouter } from 'vue-router';
import {API_URL} from "@/conf.js";

const isSupported = ref(false);
const scanIcon = ref(scan);
const router = useRouter();

const checkSupport = async () => {
  try {
    const result = await BarcodeScanner.isSupported();
    isSupported.value = result.supported;
  } catch (error) {
    console.error('Error checking if BarcodeScanner is supported:', error);
    isSupported.value = false;
  }
};

const requestPermissions = async () => {
  try {
    const { camera } = await BarcodeScanner.requestPermissions();
    return camera === 'granted' || camera === 'limited';
  } catch (error) {
    console.error('Error requesting camera permissions:', error);
    return false;
  }
};

const presentAlert = async () => {
  const alert = await alertController.create({
    header: 'Permission denied',
    message: 'Please grant camera permission to use the barcode scanner.',
    buttons: ['OK'],
  });
  await alert.present();
};

const sendBarcodeData = async (scannedBarcodes) => {
  try {
    const response = await fetch(`${API_URL}/api/Products/${scannedBarcodes[0].rawValue}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      }
    });
    const data = await response.json();
    return data;
  } catch (error) {
    console.error('Error sending barcode data:', error);
    return null;
  }
};

const scanFun = async () => {
  try {
    const granted = await requestPermissions();
    if (!granted) {
      await presentAlert();
      return;
    }
    const { barcodes: scannedBarcodes } = await BarcodeScanner.scan();
    const productData = await sendBarcodeData(scannedBarcodes);
    if (productData.status!=404) {
      router.push({ path: '/postItem', query: { name: productData.name, price: productData.price, saveProduct: null} });
    }else{
      router.push({ path: '/postItem', query: { name: productData.name, price: productData.price, saveProduct: scannedBarcodes[0].rawValue} });
    }
  } catch (error) {
    console.error('Error scanning barcode:', error);
  }
};

onMounted(() => {
  checkSupport();
});
</script>

<template>
  <ion-page>
    <ion-header>
      <ion-toolbar>
        <ion-title>Barcode Scanner</ion-title>
      </ion-toolbar>
    </ion-header>
    <ion-content>
      <div class="centered-scan-icon">
        <ion-fab-button @click="scanFun" :disabled="!isSupported" class="large-scan-button">
          <ion-icon :icon="scanIcon" size="large"></ion-icon>
        </ion-fab-button>
      </div>
    </ion-content>
  </ion-page>
</template>

<style scoped>
.centered-scan-icon {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
}

.large-scan-button {
  --background: #3880ff;
  --background-activated: #3171e0;
  --background-focused: #4c8dff;
  --background-hover: #4c8dff;
  --color: white;
  --width: 200px;
  --height: 200px;
}

.large-scan-button ion-icon {
  font-size: 100px;
}
</style>
