import React, { createContext, useContext, useState } from "react";

interface AppState {
	sidebarOpen?: boolean;
	setSidebarOpen: (open: boolean) => void;
}

export const AppStateContext = createContext<AppState>({
	sidebarOpen: true,
	setSidebarOpen: () => {}
});

export const useAppState = () => useContext(AppStateContext);

interface AppStateContextProviderProps  {
	children?: React.ReactNode;
}
export const AppStateContextProvider = (props: AppStateContextProviderProps) => {
	const [sidebarOpen, setSidebarOpen] = useState<boolean>(true);

	return (
		<AppStateContext.Provider value={{ sidebarOpen, setSidebarOpen }}>
			{props.children}
		</AppStateContext.Provider>
	);
};
