<template>
  <ion-page>
    <ion-header>
      <ion-toolbar>
        <ion-title>Items</ion-title>
        <ion-searchbar v-model="searchText" @ionChange="filterItems" placeholder="Search by name"></ion-searchbar>
      </ion-toolbar>
    </ion-header>
    <ion-content :fullscreen="true">
      <ion-list>
        <ion-item v-for="item in filteredItems" :key="item.id">
          <ion-label>
            <h2>{{ item.object }}</h2>
            <p>Price: ${{ item.price }}</p>
          </ion-label>
          <ion-button @click="navigateToEdit(item.id)" color="secondary">
            Modify
          </ion-button>
          <ion-button @click="deleteItem(item.id)" color="danger" fill="outline">
            Delete
          </ion-button>
        </ion-item>
      </ion-list>
    </ion-content>
  </ion-page>
</template>

<script setup>
import {onMounted, ref, computed} from 'vue';
import {
  IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonList, IonItem, IonLabel, IonSearchbar, IonButton
} from '@ionic/vue';
import axios from 'axios';
import {useRouter } from 'vue-router';

const items = ref([]);
const searchText = ref('');

const router = useRouter();

const navigateToEdit = (id) => {
  router.push({ name: 'EditItem', params: { id } });
};

const fetchItems = async () => {
  try {
    const response = await axios.get('https://localhost:7158/Items');
    items.value = response.data;
  } catch (error) {
    console.error('There was an error fetching the items:', error);
  }
};

const deleteItem = async (id) => {
  try {
    await axios.delete(`https://localhost:7158/Items/${id}`);
    items.value = items.value.filter(item => item.id !== id);
    alert('Item deleted successfully');
  } catch (error) {
    alert('Failed to delete item: ' + error.message);
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
