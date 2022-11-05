import { HubConnection, HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import { BackgroundTask } from "models/api-models";
import React, {
	createContext, useContext,
	useEffect,
	useState
} from "react";

interface BackgroundTaskState {
	backgroundTasks: BackgroundTask[];
}

export const BackgroundTaskContext = createContext<BackgroundTaskState>({
	backgroundTasks: [],
});

export const useBackgroundTaskState = () => useContext(BackgroundTaskContext);

interface BackgroundTaskContextProviderProps {
	children?: React.ReactNode;
}

export const BackgroundTaskContextProvider = (props: BackgroundTaskContextProviderProps) => {
	const [connection, setConnection] = useState<null | HubConnection>(null);
	const [backgroundTasks, setBackgroundTasks] = useState<
		Array<BackgroundTask>
	>([]);

	useEffect(() => {
		const connect = new HubConnectionBuilder()
			.withUrl("/hubs/backgroundtasks")
			.withAutomaticReconnect()
			.build();
		setConnection(connect);
	}, []);

	useEffect(() => {
		if (connection?.state === HubConnectionState.Disconnected) {
			connection
				.start()
				.then(() => {
					connection.on("BackgroundTasks", (tasks) => {
						setBackgroundTasks(tasks);
					});
					connection.on("BackgroundTaskStarted", (task) => {
						console.log('here!');
						setBackgroundTasks((prevTasks) => [...prevTasks, task]);
					});
					connection.on("BackgroundTaskUpdated", (task) => {
						const tasks = backgroundTasks.filter(
							(t) => t.id !== task.id
						);
						setBackgroundTasks([...tasks, task]);
					});
					connection.on("BackgroundTaskCompleted", (task) => {
						const tasks = backgroundTasks.filter(
							(t) => t.id !== task.id
						);
						setBackgroundTasks(tasks);
					});
				})
				.catch((error) => console.log(error));
		}
	}, [backgroundTasks, connection]);

	return (
		<BackgroundTaskContext.Provider value={{ backgroundTasks }}>
			{props.children}
		</BackgroundTaskContext.Provider>
	);
};
