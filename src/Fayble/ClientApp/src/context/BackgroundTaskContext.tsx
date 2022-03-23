import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { BackgroundTask } from "models/api-models";
import React, {
	createContext,
	FC,
	useContext,
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

export const BackgroundTaskContextProvider: FC = (props) => {
	const [connection, setConnection] = useState<null | HubConnection>(null);
	const [backgroundTasks, setBackgroundTasks] = useState<
		Array<BackgroundTask>
	>([]);

	useEffect(() => {
		console.log(backgroundTasks);
	}, [backgroundTasks]);

	useEffect(() => {
		const connect = new HubConnectionBuilder()
			.withUrl("/hubs/backgroundtasks")
			.withAutomaticReconnect()
			.build();
		setConnection(connect);
	}, []);

	useEffect(() => {
		if (connection) {
			connection
				.start()
				.then(() => {
					connection.on("BackgroundTasks", (tasks) => {
						setBackgroundTasks(tasks);
					});
					connection.on("BackgroundTaskStarted", (task) => {
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
	}, [connection]);

	return (
		<BackgroundTaskContext.Provider value={{ backgroundTasks }}>
			{props.children}
		</BackgroundTaskContext.Provider>
	);
};
