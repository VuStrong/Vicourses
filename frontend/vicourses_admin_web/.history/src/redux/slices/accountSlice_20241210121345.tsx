import { createSlice } from '@reduxjs/toolkit';
const storedUser = localStorage.getItem('account')
  ? JSON.parse(localStorage.getItem('account')!)
  : null;
const accountSlice = createSlice({
  name: 'account',
  initialState: {
    status: storedUser ? 'authenticated' : 'unauthenticated',
    data: {
      name: storedUser ? storedUser.name : '',
      email: storedUser ? storedUser.email : '',
      id: storedUser ? storedUser.email : '',
    },
  },
  reducers: {
    loadAccount: (state, action) => {
      const { name, email,id } = action.payload;
      state.status = 'authenticated';
      state.data.name = name;
      state.data.email = email;
      state.data.id = id;
    },
    logoutAccount: (state) => {
      state.status = 'unauthenticated';
      state.data.name = '';
      state.data.email = '';
      state.data.id = '';
    },
  },
});

export const { loadAccount } = accountSlice.actions;
export default accountSlice.reducer;
