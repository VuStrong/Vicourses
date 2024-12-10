import { configureStore } from '@reduxjs/toolkit';
import accountReducer from './slices/accountSlice'

const store = configureStore({
    reducer: {
        account:accountReducer,
    },
});
export type RootState = ReturnType<typeof store.getState>;
export default store;