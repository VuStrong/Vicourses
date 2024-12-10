
import { BACKEND_URL } from '../../libs/contants';
import callAPI from '../callApi';

export async function login(email: string, passWord: string): Promise<any> {
  console.log(BACKEND_URL);
  const res = await callAPI(`${BACKEND_URL}/api/v1/auth/login`, {
    method: 'POST',
    contentType: 'application/json',
    body: JSON.stringify({ email, passWord }),
  });

  const data = await res.json();

  if (!res.ok) {
    throw new Error(data.message ?? 'Error');
  }

  return data;
}
export async function logout(refreshToken: string) {
  console.log(BACKEND_URL);
  const res = await callAPI(`${BACKEND_URL}/api/v1/auth/logout`, {
    method: 'POST',
    contentType: 'application/json',
    body: JSON.stringify({
      refreshToken,
    }),
  });

  const data = await res.json();
  if (!res.ok) {
    throw new Error(data.message ?? 'Error');
  }

  return data;
}
