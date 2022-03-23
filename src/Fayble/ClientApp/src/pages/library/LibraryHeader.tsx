import {
	faEdit,
	faList,
	faSearchPlus,
	faTh
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import cn from "classnames";
import { Breadcrumb } from "components/breadcrumb";
import { BackgroundTaskType, BreadcrumbItem, LibraryView } from "models/ui-models";
import React from "react";
import { NavDropdown, OverlayTrigger, Tooltip } from "react-bootstrap";
import { useRunBackgroundTask } from "services";
import styles from "./LibraryHeader.module.scss";

interface LibraryHeaderProps {
	navItems: BreadcrumbItem[];
	libraryId: string;
	libraryView: LibraryView;
	openEditModal: () => void;
}

export const LibraryHeader = ({
	navItems,
	openEditModal,
	libraryView,
	libraryId,
}: LibraryHeaderProps) => {
	const runBackgroundTask = useRunBackgroundTask();

	const scan = () => {
		// if (scanTask.running && scanTask.libraryId === props.libraryId) {
		// 	toast.warn("Library scan already running");
		// 	return;
		// }
		// runTask(props.libraryId, TaskType.LibraryScan);

		runBackgroundTask.mutate([
			null,
			{
				itemId: libraryId,
				taskType: BackgroundTaskType[BackgroundTaskType.LibraryScan],
			},
		]);
	};

	return (
		<div className={cn(styles.libraryHeader)}>
			<Breadcrumb items={navItems} />
			<div className={styles.toolbar}>
				<div className={cn(styles.libraryHeaderButton)}>
					<OverlayTrigger
						overlay={<Tooltip id="hi">Edit</Tooltip>}
						placement="top">
						<FontAwesomeIcon
							icon={faEdit}
							className={styles.libraryHeaderIcon}
							onClick={openEditModal}
						/>
					</OverlayTrigger>
				</div>
				<div className={styles.libraryHeaderButton}>
					<OverlayTrigger
						overlay={<Tooltip id="hi">Scan Library</Tooltip>}
						placement="top">
						<FontAwesomeIcon
							className={styles.libraryHeaderIcon}
							icon={faSearchPlus}
							onClick={scan}
						/>
					</OverlayTrigger>
				</div>
				<div className={styles.libraryHeaderMenu}>
					<OverlayTrigger
						overlay={<Tooltip id="hi">Change View</Tooltip>}
						placement="top">
						<NavDropdown
							id="library-view-toggle"
							title={
								<FontAwesomeIcon
									className={styles.libraryHeaderIcon}
									icon={
										libraryView === LibraryView.CoverGrid
											? faTh
											: faList
									}
								/>
							}>
							<NavDropdown.Item>Cover View</NavDropdown.Item>
							<NavDropdown.Item>List View</NavDropdown.Item>
						</NavDropdown>
					</OverlayTrigger>
				</div>
			</div>
		</div>
	);
};
