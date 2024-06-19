import type { CapacitorConfig } from '@capacitor/cli';

const config: CapacitorConfig = {
  appId: 'com.example.app',
  appName: 'myApp',
  webDir: './dist',
  plugins: {
    PushNotifications:{
      presentationOptions: ["badge","sound","alert"],
    },
  },
};

export default config;
