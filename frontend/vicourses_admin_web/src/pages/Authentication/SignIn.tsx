import { yupResolver } from '@hookform/resolvers/yup';
import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import * as yup from 'yup';
import { login } from '../../service/api/auth';
import { useDispatch } from 'react-redux';
import { loadAccount } from '../../redux/slices/accountSlice';
const Login = () => {
  const [isLoading, setIsLoading] = useState(false);
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const schema = yup.object().shape({
    email: yup
      .string()
      .email('email không hợp lệ')
      .required('Không được để trống'),
    password: yup.string().required('Không được để trống'),
  });
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    defaultValues: {
      email: '',
      password: '',
    },
    resolver: yupResolver(schema),
  });
  const onSubmit = async (data: any) => {
    const { email, password } = data;
    try {
      if (isLoading) return;
      setIsLoading(true);
      const result = await login(email, password);
      if (result && result.user.role === 'admin') {
        localStorage.setItem('accessToken', result.accessToken);
        localStorage.setItem('refreshToken', result.refreshToken);
        localStorage.setItem('account', JSON.stringify(result.user));
        console.log('running')
        dispatch(loadAccount(data.user));
        console.log('running')
        navigate('/');
      } else {
        navigate('/auth/login');
      }
    } catch (error) {}
    setIsLoading(false);
  };

  return (
    <div>
      <section className="bg-gray-50 dark:bg-gray-900">
        <div className="flex flex-col items-center justify-center px-6 py-8 mx-auto md:h-screen lg:py-0">
          <div className="w-full bg-white rounded-lg shadow dark:border md:mt-0 sm:max-w-md xl:p-0 dark:bg-gray-800 dark:border-gray-700">
            <div className="p-6 space-y-4 md:space-y-6 sm:p-8">
              <h1 className="text-xl font-bold leading-tight tracking-tight text-gray-900 md:text-2xl dark:text-white">
                Đăng Nhập
              </h1>
              <form
                className="space-y-4 md:space-y-6"
                onSubmit={handleSubmit(onSubmit)}
              >
                <div>
                  <label className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">
                    Email:
                  </label>
                  <input
                    {...register('email')}
                    className="                        
                        peer
                        w-full
                        bg-white
                        text-black
                        border-2
                        rounded-md
                        outline-none
                        transition
                        disabled:opacity-70
                        disabled:cursor-not-allowed"
                  />
                  <p className='text-red-700'>{errors.email?.message}</p>
                </div>
                <div>
                  <label className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">
                    Password:
                  </label>
                  <input
                    {...register('password')}
                    type="password"
                    className="                        
                        peer
                        w-full
                        bg-white
                        text-black
                        border-2
                        rounded-md
                        outline-none
                        transition
                        disabled:opacity-70
                        disabled:cursor-not-allowed"
                  />
                  <p className='text-red-700'>{errors.password?.message}</p>
                </div>
                <div className="flex items-center justify-between">
                  <a
                    href="#"
                    className="text-sm font-medium text-gray-500 hover:underline dark:text-primary-500"
                  >
                    Quên mật khẩu?
                  </a>
                </div>
                <button
                  disabled={isLoading}
                  type="submit"
                  className=" w-full text-white bg-blue-600 hover:bg-primary-700 focus:ring-4 focus:outline-none focus:ring-primary-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-primary-600 dark:hover:bg-primary-700 dark:focus:ring-primary-800"
                >
                  {isLoading ? 'Waiting...' : 'Đăng nhập'}
                </button>
                <p className="text-sm font-light text-gray-500 dark:text-gray-400">
                  Chưa có tài khoản?{' '}
                  <a
                    href="/auth/register"
                    className="font-medium text-primary-600 hover:underline dark:text-primary-500"
                  >
                    Đăng ký
                  </a>
                </p>
              </form>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
};

export default Login;
