import { IconDefinition } from "@fortawesome/fontawesome-svg-core";
import cn from "classnames";
import React from "react";
import styles from "./SidebarSubmenuItem.module.scss";

interface SidebarSubmenuItemProps {
	id: string;
	name: string;
	icon?: IconDefinition;
	activeItem: string;
	setActive: (id: string) => void;
}

export const SidebarSubmenuItem: React.FC<SidebarSubmenuItemProps> = (
	props
) => {
	return (
		<div
			className={cn(styles.submenuItem, {
				[styles.active]: props.activeItem === props.id,
			})}
			onClick={(): void => props.setActive(props.id)}>
			{props.name}
		</div>
	);
};
