import { BACKEND_URL } from '../../libs/contants';
import { LoginResponse, RegisterDto, RegisterResponse } from '../../types/auth';
import callAPI from '../callApi';

export async function login(
  email: string,
  passWord: string,
): Promise<LoginResponse> {
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

  return data as LoginResponse;
}
export async function register(
  payload: RegisterDto,
): Promise<RegisterResponse> {
  console.log(BACKEND_URL);
  const res = await callAPI(`${BACKEND_URL}/api/v1/auth/register`, {
    method: 'POST',
    contentType: 'application/json',
    body: JSON.stringify(payload),
  });

  const data = await res.json();

  if (!res.ok) {
    throw new Error(data.message ?? 'Error');
  }

  return data as RegisterResponse;
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
export async function resetPass(
  accessToken: string,
  payLoad: {
    password: string;
    newPassword: string;
  },
) {
  console.log(BACKEND_URL);
  const res = await callAPI(`${BACKEND_URL}/api/v1/auth/reset-password`, {
    method: 'POST',
    contentType: 'application/json',
    accessToken: accessToken,
    body: JSON.stringify(payLoad),
  });

  const data = await res.json();
  if (!res.ok) {
    throw new Error(data.message ?? 'Error');
  }

  return data;
}
