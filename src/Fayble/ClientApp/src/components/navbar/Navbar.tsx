import { faBell } from "@fortawesome/free-regular-svg-icons";
import { faCircleNotch, faUserCircle } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import cn from "classnames";
import { BackgroundTaskSidebar } from "components/backgroundTaskSidebar";
import { useBackgroundTaskState } from "context";
import React, { useState } from "react";
import {
	FormControl,
	Nav,
	Navbar as RBNavbar,
	NavDropdown
} from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import styles from "./Navbar.module.scss";

export const NavbarMenu = () => {
	const navigate = useNavigate();

	const [showBackgroundTaskSidebar, setShowBackgroundTaskSidebar] =
		useState(false);

	const { backgroundTasks } = useBackgroundTaskState();
	const activeTasks = backgroundTasks && backgroundTasks.length > 0;

	const logout = () => {
		// clearAuthConfig();
		navigate("/");
	};

	return (
		<>
			<RBNavbar expand="md" className={styles.navMenu} sticky="top">
				{/* <Navbar.Brand href="#home">Fayble</Navbar.Brand> */}
				<RBNavbar.Toggle aria-controls="responsive-navbar-nav" />
				<RBNavbar.Collapse className={styles.navbarCollapse}>
					<Nav className="me-auto">
						<FormControl
							type="search"
							placeholder="Search"
							className={styles.search}
							aria-label="Search"
						/>
						{/* <Nav.Link href="#features">Features</Nav.Link>
					<Nav.Link href="#pricing">Pricing</Nav.Link> */}
						{/* <NavDropdown title="Dropdown" id="collasible-nav-dropdown">
						<NavDropdown.Item href="#action/3.1">
							Action
						</NavDropdown.Item>
						<NavDropdown.Item href="#action/3.2">
							Another action
						</NavDropdown.Item>
						<NavDropdown.Item href="#action/3.3">
							Something
						</NavDropdown.Item>
						<NavDropdown.Divider />
						<NavDropdown.Item href="#action/3.4">
							Separated link
						</NavDropdown.Item>
					</NavDropdown> */}
					</Nav>

					{/* <TasksNavDropDown /> */}
					<Nav.Link
						onClick={() =>
							setShowBackgroundTaskSidebar(
								!showBackgroundTaskSidebar
							)
						}>
						<FontAwesomeIcon
							icon={faCircleNotch}
							size="lg"
							spin={activeTasks}
							className={cn
								(styles.navMenuIcon,
								{ [styles.active]: activeTasks })
							}
						/>
					</Nav.Link>
					<NavDropdown
						align="end"
						title={
							<FontAwesomeIcon
								icon={faBell}
								size="lg"
								className={styles.navMenuIcon}
							/>
						}
						className={styles.navMenuDropdown}>
						<NavDropdown.Item>Action</NavDropdown.Item>
						<NavDropdown.Item>Action</NavDropdown.Item>
						<NavDropdown.Item>Action</NavDropdown.Item>
					</NavDropdown>
					<NavDropdown
						align="end"
						title={
							<FontAwesomeIcon
								icon={faUserCircle}
								size="lg"
								className={styles.navMenuIcon}
							/>
						}
						className={styles.navMenuDropdown}>
						<NavDropdown.Item onClick={logout}>
							Logout
						</NavDropdown.Item>
					</NavDropdown>
				</RBNavbar.Collapse>
			</RBNavbar>
			<BackgroundTaskSidebar
				show={showBackgroundTaskSidebar}
				setShow={setShowBackgroundTaskSidebar}
			/>
		</>
	);
};
