import { refreshToken } from './api/refreshToken';

interface CallApiOptions {
  method?: 'GET' | 'HEAD' | 'POST' | 'PUT' | 'PATCH' | 'DELETE';
  contentType?: 'application/json' | 'multipart/form-data';
  accessToken?: string;
  body?: any;
}

export default async function callAPI(
  endpoint: string,
  options?: CallApiOptions,
) {
  let headerContents = {} as any;

  if (options?.contentType) {
    headerContents = {
      'Content-Type': options.contentType,
    };
  }

  if (options?.accessToken) {
    headerContents = {
      ...headerContents,
      Authorization: `Bearer ${options.accessToken}`,
    };
  }

  const res = await fetch(endpoint, {
    method: options?.method ?? 'GET',
    headers: headerContents,
    body: options?.body,
  });
  if (res.status === 401 && options?.accessToken) {
    const errorData = await res.json();
    if (errorData.message === 'Token expired') {
      // Gọi hàm làm mới token
      const newAccessToken = await refreshToken();
      if (newAccessToken) {
        headerContents.Authorization = `Bearer ${newAccessToken}`
        const retryResponse = await fetch(endpoint, {
          method: options?.method ?? 'GET',
          headers: headerContents,
          body: options?.body,
        });
        return retryResponse;
      }
    }
  }
  return res;
}
