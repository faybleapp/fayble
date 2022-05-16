import { LoadingIndicator } from "components/loadingIndicator";
import React from "react";
import styles from "./PageContainer.module.scss";

interface PageContainerProps {
	children: React.ReactNode;
	loading?: boolean;
}

export const PageContainer = ({ children, loading }: PageContainerProps) => {
	return (
		<div className={styles.container}>
			{loading ? <LoadingIndicator /> : children}
		</div>
	);
};
