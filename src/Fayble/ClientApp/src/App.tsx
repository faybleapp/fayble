

import { AppStateContextProvider, BackgroundTaskContextProvider } from "context";
import { Main } from "Main";
import React from "react";
import { QueryClient, QueryClientProvider } from "react-query";
import { ReactQueryDevtools } from "react-query/devtools";
import { BrowserRouter } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import { HttpClientProvider } from "services/httpClient";
import "./scss/App.scss";

const App = () => {
	const queryClient = new QueryClient();

	return (
		<BrowserRouter>
			<QueryClientProvider client={queryClient}>
				<AppStateContextProvider>
					<BackgroundTaskContextProvider>
						<ToastContainer />
						<HttpClientProvider>
							<ReactQueryDevtools initialIsOpen={false} />
							<Main />
						</HttpClientProvider>
					</BackgroundTaskContextProvider>
				</AppStateContextProvider>
			</QueryClientProvider>
		</BrowserRouter>
	);
};

export default App;
