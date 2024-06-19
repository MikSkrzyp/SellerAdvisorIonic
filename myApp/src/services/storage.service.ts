// src/services/storage.service.ts

import { Preferences } from '@capacitor/preferences';
import { ref } from 'vue';
import { from, Observable } from 'rxjs';

export const APP_TOKEN_KEY = 'app_token';

export function useStorageService() {
    // Set a preference value
    const setStorage = (key: string, value: any) => {
        Preferences.set({ key, value });
    };

    // Get a preference value
    const getStorage = (key: string): any => {
        return Preferences.get({ key });
    };

    // Remove a preference value
    const removeStorage = (key: string) => {
        Preferences.remove({ key });
    };

    // Clear all preferences
    const clearStorage = () => {
        Preferences.clear();
    };

    // Get an observable for a preference value
    const getToken = (): Observable<any> => {
        return from(getStorage(APP_TOKEN_KEY));
    };

    return {
        setStorage,
        getStorage,
        removeStorage,
        clearStorage,
        getToken,
    };
}
