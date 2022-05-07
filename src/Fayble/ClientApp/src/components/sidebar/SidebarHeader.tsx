import { faChevronRight } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React from "react";
import styles from "./SidebarHeader.module.scss";

interface SidebarHeaderProps {}

export const SidebarHeader = ({}: SidebarHeaderProps) => {
	return (
		<div className={styles.header}>
			<span>
				<FontAwesomeIcon
					className={styles.toggleCollapseIcon}
					icon={faChevronRight}
				/>
				<h5 className={styles.brand}>Fayble</h5>
			</span>
		</div>
	);
};
