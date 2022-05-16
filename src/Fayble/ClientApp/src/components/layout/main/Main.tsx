import cn from "classnames";
import { NavbarMenu } from "components/navbar";
import { Sidebar } from "components/sidebar";
import { useAppState } from "context";
import { isAuthenticated } from "helpers/authenticationHelpers";
import { useEffect } from "react";
import { Outlet, useNavigate } from "react-router-dom";
import { useSystemConfiguration } from "services/system";
import styles from "./Main.module.scss";

export const Main = () => {
	const loggedIn = isAuthenticated();
	const navigate = useNavigate();	
	const { data: systemConfiguration } = useSystemConfiguration();

	useEffect(() => {
		if (!loggedIn) {
			navigate("/login");
		}
	}, [loggedIn, navigate, systemConfiguration]);

	const { sidebarOpen } = useAppState();
	return (
		<>
			{loggedIn && (
				<div className={styles.container}>
					<NavbarMenu />
					<div className={styles.body}>
						<div
							className={cn(
								styles.sidebar,
								sidebarOpen ? styles.open : styles.closed
							)}>
							<Sidebar />
						</div>
						<div className={styles.main}>
							<div className={styles.pageContent}>
								<Outlet />
							</div>
						</div>
					</div>
				</div>
			)}
		</>
	);
};
