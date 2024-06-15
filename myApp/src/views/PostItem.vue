<script setup>
import { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonItem, IonLabel, IonInput, IonButton } from '@ionic/vue';
import { ref } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import axios from 'axios';
import { API_URL } from "@/conf.js";

const router = useRouter();
const route = useRoute();
const item = ref({
  object: route.query.name || '',
  price: route.query.price || 0,
  save: route.query.save
});

const postItem = async () => {
  try {
    await axios.post(`${API_URL}/Items`, item.value);
    alert('Item posted successfully');

    if(item.value.save!==null){
      try {
        await axios.post(`${API_URL}/api/Products`, {
          barcode: item.value.save,
          name: item.value.object,
          price: item.value.price
        });
        alert('Product posted successfully');
      } catch (error) {
        alert('Failed to post product: ' + error.message);
      }
    }
    router.push('/').then(() => {
      location.reload();
    });
    item.value = {object: '', price: 0}; // Reset form
  } catch (error) {
    alert('Failed to post item: ' + error.message);
  }
};
</script>

<template>
  <ion-page>
    <ion-header>
      <ion-toolbar>
        <ion-title>Post an Item</ion-title>
      </ion-toolbar>
    </ion-header>
    <ion-content :fullscreen="true" class="ion-padding">
      <form @submit.prevent="postItem">
        <ion-item>
          <ion-label class="custom-label" position="floating">Object Name</ion-label>
          <ion-input v-model="item.object" required></ion-input>
        </ion-item>
        <ion-item>
          <ion-label class="custom-label" position="floating">Price</ion-label>
          <ion-input type="number" v-model.number="item.price" required></ion-input>
        </ion-item>
        <ion-button expand="block" type="submit" class="ion-margin-top">Submit</ion-button>
      </form>
    </ion-content>
  </ion-page>
</template>

<style scoped>
.custom-label {
  font-weight: bold;
  margin-bottom: 10px;
}
</style>
