import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState
} from "@microsoft/signalr";
import { BackgroundTask } from "models/api-models";
import React, { createContext, useContext, useEffect, useState } from "react";

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

export const BackgroundTaskContextProvider = (
  props: BackgroundTaskContextProviderProps
) => {
  const [connection, setConnection] = useState<null | HubConnection>(null);
  const [backgroundTasks, setBackgroundTasks] = useState<Array<BackgroundTask>>(
    []
  );

  useEffect(() => {
    const connect = new HubConnectionBuilder()
      // TODO: Port from config
      .withUrl("https://localhost:5001/hubs/backgroundtasks")
      .withAutomaticReconnect()
      .build();
    setConnection(connect);
  }, []);

  useEffect(() => {
    console.log(backgroundTasks);
  }, [backgroundTasks]);

  useEffect(() => {
    if (connection?.state === HubConnectionState.Disconnected) {
      connection
        .start()
        .then(() => {
          connection.on("BackgroundTasks", (tasks) => {
            setBackgroundTasks(tasks);
          });
          connection.on("BackgroundTaskCreated", (task) => {
            console.log(task);
            setBackgroundTasks((prevTasks) => [...prevTasks, task]);
          });
          connection.on("BackgroundTaskUpdated", (task) => {
            const tasks = backgroundTasks.filter((t) => t.id !== task.id);
            setBackgroundTasks(
              [...tasks, task].filter((t) => t.status !== "Complete")
            );
          });
          connection.on("BackgroundTaskDescriptionUpdate", (task) => {
            const updatedTask = backgroundTasks.find((t) => t.id === task.id);
            if (!updatedTask) return;
            updatedTask?.description == task.description;
            setBackgroundTasks([
              ...backgroundTasks.filter((t) => t.id !== task.id),
              { ...updatedTask, description: task.description },
            ]);
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
