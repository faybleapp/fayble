import cn from "classnames";
import { useAppState } from "context/AppStateContext";
import React from "react";
import { Spinner } from "react-bootstrap";
import styles from "./LoadingIndicator.module.scss";

export const LoadingIndicator = () => {
	const { sidebarOpen } = useAppState();
	return (
		<Spinner			
			className={cn(
				styles.spinner,
				sidebarOpen ? styles.sidebaropen : styles.sidebarclosed
			)}
			animation="border"
		/>
	);
};
