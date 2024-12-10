import { createSlice } from '@reduxjs/toolkit';
const storedUser = localStorage.getItem('account')
  ? JSON.parse(localStorage.getItem('account')!)
  : null;
const accountSlice = createSlice({
  name: 'account',
  initialState: {
    status: storedUser ? 'authenticated' : 'unauthenticated',
    data: storedUser
  },
  reducers: {
    loadAccount: (state, action) => {
      state.status = 'authenticated';
      state.data = action.payload
    },
    logoutAccount: (state) => {
      state.status = 'unauthenticated';
      state.data = null
    },
  },
});

export const { loadAccount } = accountSlice.actions;
export default accountSlice.reducer;
