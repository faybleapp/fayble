import {
	AppStateContextProvider,
	BackgroundTaskContextProvider
} from "context";
import { isAuthenticated } from "helpers/authenticationHelpers";
import { QueryClient, QueryClientProvider } from "react-query";
import { ReactQueryDevtools } from "react-query/devtools";
import { BrowserRouter } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import { HttpClientProvider } from "services/httpClient";
import { Routes } from "./Routes";
import "./scss/App.scss";

const App = () => {
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
							<Routes />
						</HttpClientProvider>
					</BackgroundTaskContextProvider>
				</AppStateContextProvider>
			</QueryClientProvider>
		</BrowserRouter>
	);
};

export default App;
