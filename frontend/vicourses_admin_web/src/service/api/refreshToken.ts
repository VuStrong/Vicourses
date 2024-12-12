import { BACKEND_URL } from '../../libs/contants';
import callAPI from '../callApi';

export async function refreshToken() {
  const refreshToken = localStorage.getItem('refreshToken');
  const account = localStorage.getItem('account') as any;
  const userId = account.id;
  try {
    const res = await callAPI(`${BACKEND_URL}/api/us/v1/auth/refresh-token`, {
      method: 'POST',
      contentType: 'application/json',
      body: JSON.stringify({ refreshToken, userId }),
    });

    if (!res.ok) {
      throw new Error('Failed to refresh token');
    }
    const data = await res?.json();
    localStorage.setItem('accessToken', data.accessToken);
    return data.accessToken;
  } catch (error) {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('account');
    return null;
  }
}
