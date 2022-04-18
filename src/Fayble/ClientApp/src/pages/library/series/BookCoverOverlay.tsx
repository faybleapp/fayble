import { faBars } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import cn from "classnames";
import React, { useState } from "react";
import { Dropdown, DropdownButton } from "react-bootstrap";
import styles from "./BookCoverOverlay.module.scss";

interface BookCoverOverlayProps {
	hideMenu: boolean;
	markRead: () => void;
	edit: () => void;
}

export const BookCoverOverlay = ({
	markRead,
	edit,
	hideMenu,
}: BookCoverOverlayProps) => {
	const [active, setActive] = useState(false);
	return (
		<div
			className={cn(styles.container, { [styles.active]: active })}
			onMouseLeave={() => setActive(false)}>
			{hideMenu? null : (
				<DropdownButton
					drop={"down"}
					className={styles.menuDropDown}
					onClick={(e: React.MouseEvent<HTMLButtonElement>) => {
						e.preventDefault();
						setActive(!active);
					}}
					title={
						<FontAwesomeIcon
							className={styles.icon}
							icon={faBars}
							// size="lg"
						/>
					}>
					<Dropdown.Item href="#/action-1">Action</Dropdown.Item>
					<Dropdown.Item href="#/action-2">
						Another action
					</Dropdown.Item>
					<Dropdown.Item href="#/action-3">
						Something else
					</Dropdown.Item>
				</DropdownButton>
			)}
		</div>
	);
};
