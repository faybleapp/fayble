import { useBackgroundTaskState } from "context";
import { BackgroundTask } from "models/api-models";
import { BackgroundTaskStatus } from "models/ui-models";
import React from "react";
import { Offcanvas, Spinner } from "react-bootstrap";
import styles from "./BackgroundTaskSidebar.module.scss";

interface BackgroundTaskSidebarProps {
	show: boolean;
	setShow: (show: boolean) => void;
}

export const BackgroundTaskSidebar = ({
	show,
	setShow,
}: BackgroundTaskSidebarProps) => {
	const { backgroundTasks } = useBackgroundTaskState();

	const runningTasks = backgroundTasks.filter(
		(task: BackgroundTask) => task.status === BackgroundTaskStatus.Running
	);

	const queuedTasks = backgroundTasks.filter(
		(task: BackgroundTask) => task.status === BackgroundTaskStatus.Queued
	);

	return (
		<Offcanvas
			show={show}
			onHide={() => setShow(false)}
			placement="end"
			className={styles.backgroundTaskSidebar}>
			<Offcanvas.Header closeButton closeVariant="white">
				<Offcanvas.Title className={styles.header}>
					Tasks
				</Offcanvas.Title>
			</Offcanvas.Header>
			<Offcanvas.Body className={styles.body}>
				<h6 className={styles.taskHeading}>Running Tasks</h6>
				<div className={styles.taskSection}>
					{runningTasks.length > 0
						? runningTasks.map((task) => {
								return (
									<>
										<div>
											{task.type
												.replace(/([A-Z])/g, " $1")
												.trim()}
											: {task.itemName}
										</div>
										<div className={styles.taskDescription}>
											- {task.description}...
											<Spinner
												animation="border"
												size="sm"
											/>
										</div>
									</>
								);
						  })
						: "There are no running tasks"}
				</div>

				<h6 className={styles.taskHeading}>Queued Tasks</h6>
				<div className={styles.taskSection}>
					{queuedTasks.length > 0
						? queuedTasks.map((task) => {
								return (
									<div>
										{task.type
											.replace(/([A-Z])/g, " $1")
											.trim()}
									</div>
								);
						  })
						: "There are no queued tasks"}
				</div>
			</Offcanvas.Body>
		</Offcanvas>
	);
};
