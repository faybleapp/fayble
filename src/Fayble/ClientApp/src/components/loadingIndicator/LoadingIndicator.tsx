import cn from "classnames";
import { useAppState } from "context";
import React from "react";
import { Spinner } from "react-bootstrap";
import styles from "./LoadingIndicator.module.scss";

export const LoadingIndicator = () => {
	const { sidebarOpen } = useAppState();

	return (
		<Spinner
			className={cn(styles.spinner, {
				[styles.sidebarOpen]: sidebarOpen,
				[styles.sidebarClosed]: !sidebarOpen,
			})}
			animation="border"
		/>
	);
};
