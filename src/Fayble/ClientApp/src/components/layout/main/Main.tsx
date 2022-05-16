import cn from "classnames";
import { Sidebar } from "components/sidebar";
import { useAppState } from "context";
import React from "react";
import { Navbar } from "react-bootstrap";
import styles from "./Main.module.scss";

interface MainProps {
	children: React.ReactNode;
}

export const Main = ({ children }: MainProps) => {
	const { sidebarOpen } = useAppState();
	return (
		<div className={styles.container}>
			<Navbar />
			<div className={styles.body}>
				<div
					className={cn(
						styles.sidebar,
						sidebarOpen ? styles.open : styles.closed
					)}>
					<Sidebar />
				</div>
				<div className={styles.main}>
					<div className={styles.pageContent}>
						{children}
					</div>
				</div>
			</div>
		</div>
	);
};
