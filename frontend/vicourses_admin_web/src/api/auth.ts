import { SignInResponse } from "../types/user";
import instance from "./axios";

export async function login(email: string, password: string): Promise<SignInResponse> {
  const res = await instance.post(`/api/us/v1/auth/login`, {
    email,
    password,
  });

  return res.data;
}

export async function logout(refreshToken: string, userId: string) {
    await instance.post(`/api/us/v1/auth/revoke-refresh-token`, {
        refreshToken,
        userId
    });
}

