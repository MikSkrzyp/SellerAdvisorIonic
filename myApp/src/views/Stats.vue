<script setup>
import { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonCard, IonCardHeader, IonCardTitle, IonCardContent } from '@ionic/vue';
import { ref, onMounted } from 'vue';
import Chart from 'chart.js/auto';
import axios from 'axios';
import {API_URL} from "@/conf.js";

const stats = ref({
  average: 0,
  median: 0,
  stdDev: 0
});
const chartRef = ref(null);

// Fetch items and calculate statistics
const fetchItemsAndSetupChart = async () => {
  try {
    const response = await axios.get(`${API_URL}/Items`);
    const items = response.data;
    calculateStatistics(items);
    setupChart(items);
  } catch (error) {
    console.error('Error fetching items:', error);
  }
};

// Calculate statistics
const calculateStatistics = (items) => {
  const prices = items.map(item => item.price);
  stats.value.average = prices.reduce((a, b) => a + b, 0) / prices.length;
  const mid = Math.floor(prices.length / 2);
  const nums = [...prices].sort((a, b) => a - b);
  stats.value.median = prices.length % 2 !== 0 ? nums[mid] : (nums[mid - 1] + nums[mid]) / 2;
  const mean = stats.value.average;
  stats.value.stdDev = Math.sqrt(prices.map(x => Math.pow(x - mean, 2)).reduce((a, b) => a + b) / prices.length);
};

// Setup chart
const setupChart = (items) => {
  const ctx = document.getElementById('priceChart').getContext('2d');
  const productNames = items.map(item => item.object); // Product names for labels
  const productPrices = items.map(item => item.price); // Prices as data points

  // Destroy previous chart instance if exists
  if (chartRef.value) {
    chartRef.value.destroy();
  }

  chartRef.value = new Chart(ctx, {
    type: 'line',
    data: {
      labels: productNames,
      datasets: [
        {
          label: 'Price',
          data: productPrices,
          borderColor: 'rgb(75, 192, 192)',
          borderWidth: 2,
          fill: false
        },
        {
          label: 'Average Price',
          data: productNames.map(() => stats.value.average), // Constant line for average
          borderColor: 'rgb(255, 159, 64)',
          borderDash: [5, 5], // Dashed line
          borderWidth: 2,
          fill: false
        },
        {
          label: 'Median Price',
          data: productNames.map(() => stats.value.median), // Constant line for median
          borderColor: 'rgb(54, 162, 235)',
          borderDash: [5, 5], // Dashed line
          borderWidth: 2,
          fill: false
        },
        {
          label: 'Standard Deviation',
          data: productNames.map(() => stats.value.stdDev), // Constant line for standard deviation
          borderColor: 'rgb(153, 102, 255)',
          borderDash: [5, 5], // Dashed line
          borderWidth: 2,
          fill: false
        }
      ]
    },
    options: {
      scales: {
        y: {
          beginAtZero: true,
          title: {
            display: true,
            text: 'Price ($)'
          }
        },
        x: {
          title: {
            display: true,
            text: 'Product'
          }
        }
      },
      plugins: {
        legend: {
          display: true,
          position: 'top',
        }
      },
      responsive: true,
      maintainAspectRatio: false
    }
  });
};


onMounted(fetchItemsAndSetupChart);
</script>

<template>
  <ion-page>
    <ion-header>
      <ion-toolbar>
        <ion-title>Product Statistics</ion-title>
      </ion-toolbar>
    </ion-header>
    <ion-content class="ion-padding">
      <!-- Statistics Card for Average, Median, STD -->
      <ion-card>
        <ion-card-header>
          <ion-card-title>Aggregate Statistics</ion-card-title>
        </ion-card-header>
        <ion-card-content>
          <p>Average Price: ${{ stats.average.toFixed(2) }}</p>
          <p>Median Price: ${{ stats.median.toFixed(2) }}</p>
          <p>Standard Deviation: ${{ stats.stdDev.toFixed(2) }}</p>
        </ion-card-content>
      </ion-card>
      <!-- Chart Section -->
      <ion-card>
        <ion-card-header>
          <ion-card-title>Price per Product</ion-card-title>
        </ion-card-header>
        <ion-card-content>
          <canvas id="priceChart"></canvas>
        </ion-card-content>
      </ion-card>
    </ion-content>
  </ion-page>
</template>



<style scoped>
canvas {
  height: 400px;
}
</style>
