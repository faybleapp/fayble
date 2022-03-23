import cn from "classnames";
import React from "react";
import { Tabs } from "react-bootstrap";
import styles from "./ModalTabs.module.scss";

interface ModalTabsProps {
	defaultActiveKey: string;
	activeTab?: string;
	onChange?: (key: string | null) => void;
    children: React.ReactNode;
}

export const ModalTabs = ({
	defaultActiveKey,
	activeTab,
	onChange,
    children
}: ModalTabsProps) => {
	return (
		<Tabs
			onSelect={onChange}
			activeKey={activeTab}
			defaultActiveKey={defaultActiveKey}
			className={cn(styles.tabs, "nav-justified")}>
			{children}
		</Tabs>
	);
};
