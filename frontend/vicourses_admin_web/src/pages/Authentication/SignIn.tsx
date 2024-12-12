import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { login, logout } from '../../service/api/auth';
import { useDispatch, useSelector } from 'react-redux';
import { loadAccount } from '../../redux/slices/accountSlice';

type FormData = {
  email: string;
  password: string;
};

const emailRegex = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|.(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

const Login = () => {
  const [isLoading, setIsLoading] = useState(false);
  const account = useSelector((state: any) => state.account.data);
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormData>();

  useEffect(() => {
    if (account) {
      navigate('/');
    }
  }, [account]);

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

        dispatch(loadAccount(data.user));

        navigate('/');
      } else {
        
        if (result) await logout(result.refreshToken, result.user.id);
        alert("Forbidden");
      }
    } catch (error: any) {
      alert(error.message);
    }
    setIsLoading(false);
  };
  return (
    <div>
      <section className="bg-gray-50 dark:bg-gray-900">
        <div className="flex flex-col items-center justify-center px-6 py-8 mx-auto md:h-screen lg:py-0">
          <div className="w-full bg-white rounded-lg shadow dark:border md:mt-0 sm:max-w-md xl:p-0 dark:bg-gray-800 dark:border-gray-700">
            <div className="p-6 space-y-4 md:space-y-6 sm:p-8">
              <h1 className="text-xl font-bold leading-tight tracking-tight text-gray-900 md:text-2xl dark:text-white">
                Login
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
                    {...register('email', {
                      required: {
                        value: true,
                        message: "Enter email"
                      },
                      pattern:{
                        value: emailRegex,
                        message: "Email invalid"
                      }                        
                    })}
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
                  <p className="text-red-700">{errors.email?.message}</p>
                </div>
                <div>
                  <label className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">
                    Password:
                  </label>
                  <input
                    {...register('password', { required: true })}
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
                  <p className="text-red-700">{errors.password?.message}</p>
                </div>
                <button
                  disabled={isLoading}
                  type="submit"
                  className=" w-full text-white bg-blue-600 hover:bg-primary-700 focus:ring-4 focus:outline-none focus:ring-primary-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-primary-600 dark:hover:bg-primary-700 dark:focus:ring-primary-800"
                >
                  {isLoading ? 'Waiting...' : 'Login'}
                </button>
              </form>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
};

export default Login;
