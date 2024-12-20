import { create } from 'zustand';
import { User } from '../types/user';
import instance from '../api/axios';

interface AccountStore {
  status: 'unauthenticated' | 'authenticated' | 'loading';
  data: User | null;

  initialize: () => Promise<void>;
}

const useAccount = create<AccountStore>((set) => ({
  status: 'loading',
  data: null,

  initialize: async () => {
    const dataString = localStorage.getItem('token_data');

    if (!dataString) {
      return set({
        status: 'unauthenticated',
      });
    }

    try {
        const res = await instance.get<User>("/api/us/v1/me");

        return set({
            status: "authenticated",
            data: res.data,
        }); 
    } catch (error) {
      console.log(error);
      
        return set({
            status: "unauthenticated"
        });        
    }
  },
}));

export default useAccount;
