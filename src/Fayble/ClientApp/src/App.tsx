import {
	AppStateContextProvider,
	BackgroundTaskContextProvider
} from "context";
import { isAuthenticated } from "helpers/authenticationHelpers";
import { Main } from "Main";
import { Login } from "pages/login";
import React from "react";
import { QueryClient, QueryClientProvider } from "react-query";
import { ReactQueryDevtools } from "react-query/devtools";
import { BrowserRouter } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import { HttpClientProvider } from "services/httpClient";
import { useSystemConfiguration } from "services/system";
import "./scss/App.scss";

const App = () => {
	const [loading, setLoading] = useState<boolean>(true);
	const {
		data: systemConfiguration,
		isLoading: isLoadingSystemConfiguration,
	} = useSystemConfiguration();

	const queryClient = new QueryClient();
	const loggedIn = isAuthenticated();

	return (
		<BrowserRouter>
			<QueryClientProvider client={queryClient}>
				<ReactQueryDevtools initialIsOpen={false} />
				<AppStateContextProvider>
					<BackgroundTaskContextProvider>
						<ToastContainer
							position="bottom-right"
							closeOnClick
							theme="dark"
						/>
						<HttpClientProvider>
							{loggedIn ? <Main /> : <Login />}
						</HttpClientProvider>
					</BackgroundTaskContextProvider>
				</AppStateContextProvider>
			</QueryClientProvider>
		</BrowserRouter>
	);
};

export default App;
