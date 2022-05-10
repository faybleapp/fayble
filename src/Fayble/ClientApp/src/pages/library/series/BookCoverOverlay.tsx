import { faBars } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import cn from "classnames";
import { useState } from "react";
import { Dropdown, DropdownButton } from "react-bootstrap";
import styles from "./BookCoverOverlay.module.scss";
interface BookCoverOverlayProps {
	hideMenu: boolean;
	markRead: () => void;
	edit: () => void;
	deleted: boolean;
}

export const BookCoverOverlay = ({
	markRead,
	edit,
	deleted,
	hideMenu,
}: BookCoverOverlayProps) => {
	const [active, setActive] = useState(false);
	return (
		<div
			className={cn(styles.overlay, { [styles.deleted]: deleted })}
			onMouseLeave={() => setActive(false)}>
			{hideMenu ? null : (
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
