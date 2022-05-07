import axios, { AxiosInstance } from "axios";
import { getAuthConfig, setAuthConfig } from "helpers/authenticationHelpers";
import { AuthenticationResult } from "models/api-models";
import React, { useContext } from "react";

export const HttpClientContext = React.createContext<AxiosInstance | undefined>(
	undefined
);

interface HttpClientProviderProps {
	children?: React.ReactNode;
}

export const HttpClientProvider: React.FC<HttpClientProviderProps> = ({
	children,
}) => {
	return (
		<HttpClientContext.Provider value={useNewHttpClient()}>
			{children}
		</HttpClientContext.Provider>
	);
};

export const useHttpClient = () => {
	const context = useContext(HttpClientContext);
	if (!context)
		throw new Error(
			"The useHttpClient hook must be used within an <HttpClientProvider/>"
		);
	return context;
};

export const useNewHttpClient = (): AxiosInstance => {
	const httpClient = axios.create({
		baseURL: "/api",
		headers: {
			Accept: "application/json",
		},
	});

	// Add auth token
	httpClient.interceptors.request.use(
		async (request) => {
			const authConfig = getAuthConfig();
			request.headers = {
				Authorization: `Bearer ${authConfig.token}`,
			};

			return request;
		},
		(error) => {
			Promise.reject(error);
		}
	);

	httpClient.interceptors.response.use(
		async (response) => {
			return response;
		},
		(error) => {
			const originalRequest = error.config;
			const authConfig = getAuthConfig();

			// If 401 trying to login, don't retry
			if (
				error.response.status === 401 &&
				originalRequest.url
					.toLowerCase()
					.includes("/authentication/login")
			) {
				return Promise.reject(error);
			}

			// If 401 trying to refresh, go back to login
			if (
				error.response.status === 401 &&
				originalRequest.url
					.toLowerCase()
					.includes("/authentication/refresh")
			) {
				console.log("401 error: refresh failed");
				localStorage.clear();
				window.location.href = "/login";
				return Promise.reject(error);
			}

			if (
				error.response.status === 401 &&
				!originalRequest._retry &&
				authConfig
			) {
				originalRequest._retry = true;
				const refreshToken = authConfig.refreshToken;
				return httpClient
					.post("/authentication/refresh",{refreshToken: refreshToken})
					.then((response) => {
						// 1) put token to LocalStorage
						const authConfig =
							response.data as AuthenticationResult;
						setAuthConfig(authConfig);

						// 2) Change Authorization header
						originalRequest.headers = {
							Authorization: `Bearer ${authConfig.token}`,
						};

						// 3) return originalRequest object with Axios.
						return axios(originalRequest);
					});
			}
			return Promise.reject(error);
		}
	);

	return httpClient;
};
