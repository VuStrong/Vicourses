import { configureStore } from '@reduxjs/toolkit';
import sideBarReducer from './slices/sideBarSlice';
import accountReducer from './slices/accountSlice'

const store = configureStore({
    reducer: {
        sideBar: sideBarReducer,
        account:accountReducer,
    },
});
export type RootState = ReturnType<typeof store.getState>;
export default store;