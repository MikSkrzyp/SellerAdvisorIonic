<script setup>
import {onMounted, ref, computed} from 'vue';
import {
  IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonList, IonItem, IonLabel, IonSearchbar, IonButton
} from '@ionic/vue';
import axios from 'axios';
import {useRouter } from 'vue-router';
import  '../theme/variables.css'
import {API_URL} from "@/conf.js";
import {BarcodeScanner} from "@capacitor-mlkit/barcode-scanning";

const items = ref([]);
const searchText = ref('');
const isSupported = ref(false);

const router = useRouter();

const navigateToEdit = (id) => {
  router.push({ name: 'EditItem', params: { id } });
};

const navigateToStats = () => {
  router.push({ name: 'StatsView' });
};

const navigateToPostItem = () => {
  router.push('/postItem');
};

const navigateToScanner = () => {
  router.push({ name: 'ScannerView' });
};

const fetchItems = async () => {
  try {
    await checkSupport();
    const response = await axios.get(`${API_URL}/Items`);
    items.value = response.data;
  } catch (error) {
    console.error('There was an error fetching the items:', error);
  }
};

const deleteItem = async (id) => {
  try {
    await axios.delete(`${API_URL}/Items/${id}`);
    items.value = items.value.filter(item => item.id !== id);
    alert('Item deleted successfully');
  } catch (error) {
    alert('Failed to delete item: ' + error.message);
  }
};

const checkSupport = async () => {
  try {
    const result = await BarcodeScanner.isSupported();
    isSupported.value = result.supported;
  } catch (error) {
    console.error('Error checking if BarcodeScanner is supported:', error);
    isSupported.value = false;
  }
};

onMounted(fetchItems);

const filteredItems = computed(() => {
  if (!searchText.value) return items.value;
  return items.value.filter(item =>
      item.object.toLowerCase().includes(searchText.value.toLowerCase())
  );
});

</script>

<template>
  <ion-page class="page-container">
    <ion-header>
      <ion-toolbar>
        <ion-title class="toolbar-title">Items</ion-title>
      </ion-toolbar>
      <ion-toolbar>
        <ion-searchbar v-model="searchText" @ionChange="filterItems" placeholder="Search by name" class="search-bar"></ion-searchbar>
      </ion-toolbar>
      <ion-toolbar>
        <ion-buttons slot="start">
          <ion-button @click="navigateToPostItem">Add Item</ion-button>
          <ion-button v-if="isSupported" @click="navigateToScanner">Scanner</ion-button>
          <ion-button @click="navigateToStats">View Stats</ion-button>
        </ion-buttons>
      </ion-toolbar>
    </ion-header>
    <ion-content :fullscreen="true">
      <ion-list>
        <ion-item v-for="item in filteredItems" :key="item.id">
          <ion-label class="item-label">
            <h2>{{ item.object }}</h2>
            <p>Price: ${{ item.price }}</p>
          </ion-label>
          <div class="item-buttons">
            <ion-button @click="navigateToEdit(item.id)" color="secondary">
              Modify
            </ion-button>
            <ion-button @click="deleteItem(item.id)" color="danger" fill="outline">
              Delete
            </ion-button>
          </div>
        </ion-item>
      </ion-list>
    </ion-content>
  </ion-page>
</template>


