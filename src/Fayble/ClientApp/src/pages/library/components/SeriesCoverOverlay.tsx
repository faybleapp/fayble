import { faBars } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import cn from "classnames";
import React, { useState } from "react";
import { Dropdown, DropdownButton } from "react-bootstrap";
import styles from "./SeriesCoverOverlay.module.scss";

interface SeriesCoverOverlayProps {
	markRead: () => void;
	edit: () => void;
}

export const SeriesCoverOverlay = ({
	markRead,
	edit,
}: SeriesCoverOverlayProps) => {
	const [active, setActive] = useState(false);
	return (
		<div
			className={cn(styles.container, { [styles.active]: active })}
			onMouseLeave={() => setActive(false)}>
			<DropdownButton
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
				<Dropdown.Item href="#/action-2">Another action</Dropdown.Item>
				<Dropdown.Item href="#/action-3">Something else</Dropdown.Item>
			</DropdownButton>
		</div>
	);
};
