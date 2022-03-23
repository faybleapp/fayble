import { IconDefinition } from "@fortawesome/fontawesome-svg-core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import cn from "classnames";
import React from "react";
import styles from "./SidebarMenuItem.module.scss";


interface SidebarMenuItemProps {
	id: string;
	name: string;
	collapsed: boolean;
	icon: IconDefinition;
	activeIcon?: IconDefinition;
	activeItem: string;
	setActive: (id: string) => void;
}

export const SidebarMenuItem = ({
	id,
	name,
	collapsed,
	icon,
	activeIcon,
	activeItem,
	setActive,
}:SidebarMenuItemProps ) => {
	return (
		<div
			className={cn(styles.item, {
				[styles.active]: activeItem === id,
			})}
			onClick={(): void => {
				setActive(id);
			}}>
			<span>
				<FontAwesomeIcon
					icon={
						activeItem === id &&
						activeIcon !== undefined
							? activeIcon
							: icon
					}
					fixedWidth
					className={styles.icon}
				/>
			</span>
			<div
				className={cn(styles.name, {
					[styles.hide]: collapsed,
				})}>
				{name}
			</div>
		</div>
	);
};
