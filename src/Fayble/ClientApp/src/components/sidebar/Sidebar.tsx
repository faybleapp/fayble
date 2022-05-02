import {
	faBook,
	faBookOpen,
	faDesktop
} from "@fortawesome/free-solid-svg-icons";
import { LibraryModal } from "components/libraryModal";
import { useAppState } from "context/AppStateContext";
import { Library } from "models/api-models";
import React, { useState } from "react";
import { Link } from "react-router-dom";
import { useLibraries } from "services/library";
import styles from "./Sidebar.module.scss";
import { SidebarMenuItem } from "./SidebarMenuItem";
import { SidebarMenuItemDropdown } from "./SidebarMenuItemDropdown";
import { SidebarSubmenuItem } from "./SidebarSubmenuItem";

export const Sidebar = () => {
	const { data: libraries } = useLibraries();

	const { sidebarOpen } = useAppState();

	const [showLibraryModal, setShowLibraryModal] = useState(false);
	const [activeMenuItem, setActiveMenuItem] = useState("");
	const [sidebarPinned, setSidebarPinned] = useState(false);

	return (
		<>
			<div
			>
				<ul>
					{libraries &&
						libraries?.map((library: Library) => {
							return (
								<li key={library.id}>
									<Link
										to={`/library/${library.id}`}
										style={{ textDecoration: "none" }}>
										<SidebarMenuItem
											name={library.name || ""}
											collapsed={
												!sidebarPinned && !sidebarOpen
											}
											id={library.id || ""}
											icon={faBook}
											activeIcon={faBookOpen}
											setActive={setActiveMenuItem}
											activeItem={activeMenuItem}
										/>
									</Link>
								</li>
							);
						})}
					<li>
						<SidebarMenuItemDropdown
							id="application"
							name="Application"
							collapsed={!sidebarPinned && !sidebarOpen}
							activeItem={activeMenuItem}
							icon={faDesktop}>
							<div
								className={styles.nonActivatable}
								onClick={(): void => setShowLibraryModal(true)}>
								Add Library
							</div>
							<SidebarSubmenuItem
								id="update"
								name="Update"
								activeItem={activeMenuItem}
								setActive={setActiveMenuItem}
							/>
						</SidebarMenuItemDropdown>
					</li>
				</ul>
			</div>
			<LibraryModal
				show={showLibraryModal}
				close={(): void => setShowLibraryModal(false)}
			/>
		</>
	);
};
