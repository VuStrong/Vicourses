import { BACKEND_URL } from '../../libs/contants';
import callAPI from '../callApi';

export async function login(email: string, password: string): Promise<any> {
  console.log(BACKEND_URL);
  const res = await callAPI(`${BACKEND_URL}/api/us/v1/auth/login`, {
    method: 'POST',
    contentType: 'application/json',
    body: JSON.stringify({ email, password }),
  });

  const data = await res.json();

  if (!res.ok) {
    throw new Error(data.message ?? 'Error');
  }

  return data;
}
export async function logout(refreshToken: string, userId: string) {
  console.log(BACKEND_URL);
  const res = await callAPI(`${BACKEND_URL}/api/us/v1/auth/revoke-refresh-token`, {
    method: 'POST',
    contentType: 'application/json',
    body: JSON.stringify({
      refreshToken,
      userId
    }),
  });

  const data = await res.json();
  if (!res.ok) {
    throw new Error(data.message ?? 'Error');
  }

  return data;
}
