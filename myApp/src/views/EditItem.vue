<template>
  <ion-page>
    <ion-header>
      <ion-toolbar>
        <ion-title>Edit Item</ion-title>
      </ion-toolbar>
    </ion-header>
    <ion-content :fullscreen="true" class="ion-padding">
      <form @submit.prevent="updateItem">
        <ion-item>
          <ion-label position="floating">Object Name</ion-label>
          <ion-input v-model="item.object" required></ion-input>
        </ion-item>
        <ion-item>
          <ion-label position="floating">Price</ion-label>
          <ion-input type="number" v-model.number="item.price" required></ion-input>
        </ion-item>
        <ion-button expand="block" type="submit" class="ion-margin-top">Update</ion-button>
      </form>
    </ion-content>
  </ion-page>
</template>

<script setup>
import { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonItem, IonLabel, IonInput, IonButton } from '@ionic/vue';
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import axios from 'axios';

const route = useRoute();
const item = ref({ id: route.params.id, object: '', price: 0 });

const fetchItem = async () => {
  try {
    const response = await axios.get(`https://localhost:7158/Items/${item.value.id}`);
    item.value = response.data;
  } catch (error) {
    alert('Failed to load item: ' + error.message);
  }
};

const updateItem = async () => {
  try {
    await axios.put(`https://localhost:7158/Items/${item.value.id}`, item.value);
    alert('Item updated successfully');
  } catch (error) {
    alert('Failed to update item: ' + error.message);
  }
};

onMounted(fetchItem);
</script>
