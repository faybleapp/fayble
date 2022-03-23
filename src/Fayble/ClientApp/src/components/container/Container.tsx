import { LoadingIndicator } from "components/loadingIndicator";
import React from "react";
import styles from "./Container.module.scss";

interface ContainerProps {
	children: React.ReactNode;
	loading?: boolean;
}

export const Container = ({ children, loading }: ContainerProps) => {
	return (
		<div className={styles.container}>
			{loading ? <LoadingIndicator /> : children}
		</div>
	);
};
