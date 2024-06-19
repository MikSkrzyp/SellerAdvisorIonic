// src/services/pushNotification.ts

import { Capacitor } from '@capacitor/core';
import { PushNotifications, PushNotificationSchema, Token } from '@capacitor/push-notifications';
import { ref } from 'vue';

export const FCM_TOKEN_KEY = 'push_notification_token';

export function useFcmService() {
    const redirect = ref<any>(null);

    // Initialize push notifications
    const initPush = () => {
        if (Capacitor.getPlatform() !== 'web') {
            registerPush();
        }
    };

    // Register for push notifications
    const registerPush = async () => {
        try {
            await addListeners();
            let permStatus = await PushNotifications.checkPermissions();

            if (permStatus.receive === 'prompt') {
                permStatus = await PushNotifications.requestPermissions();
            }

            if (permStatus.receive !== 'granted') {
                throw new Error('User denied permissions!');
            }

            await PushNotifications.register();
        } catch (e) {
            console.error('Error registering for push notifications:', e);
        }
    };

    // Add event listeners for push notifications
    const addListeners = async () => {
        PushNotifications.addListener('registration', async (token: Token) => {
            console.log('Push registration success, token: ' + token.value);
            const fcmToken = token.value;
            saveFcmToken(fcmToken);
        });

        PushNotifications.addListener('registrationError', (error: any) => {
            console.error('Push registration error:', error);
        });

        PushNotifications.addListener('pushNotificationReceived', (notification: PushNotificationSchema) => {
            console.log('Push received:', notification);
            handlePushNotification(notification);
        });

        PushNotifications.addListener('pushNotificationActionPerformed', (notification: any) => {
            console.log('Push action performed:', notification);
            handlePushAction(notification);
        });
    };

    // Handle received push notification
    const handlePushNotification = (notification: PushNotificationSchema) => {
        const data = notification.data;
        if (data?.redirect) {
            redirect.value = data.redirect;
        }
    };

    // Handle action performed on push notification
    const handlePushAction = (notification: any) => {
        const data = notification.notification.data;
        if (data?.redirect) {
            redirect.value = data.redirect;
        }
    };

    // Save FCM token to local storage
    const saveFcmToken = (token: string) => {
        localStorage.setItem(FCM_TOKEN_KEY, token);
    };

    // Remove FCM token from local storage
    const removeFcmToken = () => {
        localStorage.removeItem(FCM_TOKEN_KEY);
    };

    return {
        redirect,
        initPush,
        removeFcmToken,
    };
}
