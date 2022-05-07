import {
	faChevronRight,
	IconDefinition
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import cn from "classnames";
import { useAppState } from "context";
import React, { useEffect, useState } from "react";
import { Collapse } from "react-bootstrap";
import styles from "./SidebarMenuItemDropdown.module.scss";

interface SidebarMenuItemDropdownProps {
	id: string;
	name: string;
	collapsed: boolean;
	icon: IconDefinition;
	activeItem: string;
	children: React.ReactNode;
}

export const SidebarMenuItemDropdown = ({
	id,
	name,
	collapsed,
	icon,
	activeItem,
	children,
}: SidebarMenuItemDropdownProps) => {
	const [expanded, setExpanded] = useState(false);
	const { setSidebarOpen } = useAppState();

	useEffect(() => {
		if (collapsed) {
			setExpanded(false);
		}
	}, [collapsed]);

	return (
		<div
			className={styles.dropdown}
			onClick={() => {
				setSidebarOpen(true);
			}}>
			<div
				className={cn(styles.libraryMenu, {
					[styles.active]: activeItem === id,
				})}
				onClick={(): void => setExpanded(!expanded)}
				aria-controls="library-submenu"
				aria-expanded={expanded}>
				<span>
					<FontAwesomeIcon
						className={styles.icon}
						icon={icon}
						fixedWidth
					/>
				</span>
				<div
					className={cn(styles.name, {
						[styles.hide]: collapsed,
					})}>
					{name}
				</div>
				<span>
					<FontAwesomeIcon
						icon={faChevronRight}
						className={cn(styles.chevron, {
							[styles.hide]: collapsed,
						})}
					/>
				</span>
			</div>
			<Collapse in={expanded}>
				<div
					className={cn(styles.submenu, {
						[styles.hide]: collapsed,
					})}>
					{children}
				</div>
			</Collapse>
		</div>
	);
};
