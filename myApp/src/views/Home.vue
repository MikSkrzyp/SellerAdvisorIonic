<template>
  <ion-page class="page-container">
    <ion-header>
      <ion-toolbar>
        <ion-title class="toolbar-title">Items</ion-title>
        <ion-searchbar v-model="searchText" @ionChange="filterItems" placeholder="Search by name" class="search-bar"></ion-searchbar>
        <ion-buttons slot="end">
          <ion-button @click="navigateToPostItem">Add Item</ion-button>
        </ion-buttons>
        <ion-button slot="end" @click="navigateToStats">View Stats</ion-button>
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

<script setup>
import {onMounted, ref, computed} from 'vue';
import {
  IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonList, IonItem, IonLabel, IonSearchbar, IonButton
} from '@ionic/vue';
import axios from 'axios';
import {useRouter } from 'vue-router';
import '../Css/Home.css'
import  '../theme/variables.css'

const items = ref([]);
const searchText = ref('');

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
