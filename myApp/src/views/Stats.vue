<template>
  <ion-page>
    <ion-header>
      <ion-toolbar>
        <ion-title>Product Statistics</ion-title>
      </ion-toolbar>
    </ion-header>
    <ion-content class="ion-padding">
      <ion-card>
        <ion-card-header>
          <ion-card-title>Statistics</ion-card-title>
        </ion-card-header>
        <ion-card-content>
          <p>Average Price: ${{ stats.average.toFixed(2) }}</p>
          <p>Median Price: ${{ stats.median.toFixed(2) }}</p>
          <p>Standard Deviation: ${{ stats.stdDev.toFixed(2) }}</p>
        </ion-card-content>
      </ion-card>
    </ion-content>
  </ion-page>
</template>

<script setup>
import { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonCard, IonCardHeader, IonCardTitle, IonCardContent } from '@ionic/vue';
import { ref, onMounted } from 'vue';
import axios from 'axios';

const stats = ref({
  average: 0,
  median: 0,
  stdDev: 0
});

const calculateStatistics = (items) => {
  const prices = items.map(item => item.price).sort((a, b) => a - b);
  const average = prices.reduce((acc, cur) => acc + cur, 0) / prices.length;
  const median = prices.length % 2 === 0 ?
      (prices[prices.length / 2 - 1] + prices[prices.length / 2]) / 2 :
      prices[Math.floor(prices.length / 2)];
  const stdDev = Math.sqrt(prices.map(price => Math.pow(price - average, 2)).reduce((acc, cur) => acc + cur, 0) / prices.length);

  return { average, median, stdDev };
};

const fetchItems = async () => {
  try {
    const response = await axios.get('https://localhost:7158/Items');
    stats.value = calculateStatistics(response.data);
  } catch (error) {
    console.error('Error fetching items:', error);
  }
};

onMounted(fetchItems);
</script>
