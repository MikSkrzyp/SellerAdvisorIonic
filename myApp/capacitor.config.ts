import type { CapacitorConfig } from '@capacitor/cli';

const config: CapacitorConfig = {
  appId: 'com.example.app',
  appName: 'sellerAdvisor',
  webDir: './dist',
  plugins: {
    PushNotifications:{
      presentationOptions: ["badge","sound","alert"],
    },
  },
};

export default config;
