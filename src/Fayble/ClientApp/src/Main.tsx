import cn from "classnames";
import { LoadingIndicator } from "components/loadingIndicator";
import { NavbarMenu as Navbar } from "components/navbar";
import { Sidebar } from "components/sidebar";
import { useAppState } from "context";
import { isAuthenticated } from "helpers/authenticationHelpers";
import { FirstRun } from "pages/first-run";
import { Home } from "pages/home";
import { Library } from "pages/library";
import { Book } from "pages/library/book";
import { Series } from "pages/library/series";
import { Login } from "pages/login";
import { useEffect, useState } from "react";
import { Route, Routes, useLocation, useNavigate } from "react-router-dom";
import { useSystemConfiguration } from "services/system";
import styles from "./Main.module.scss";

export const Main = () => {
	const [hideNavAndSidebar, setHideNavAndSidebar] = useState<boolean>(true);
	const [loading, setLoading] = useState<boolean>(true);

	const loggedIn = isAuthenticated();
	const navigate = useNavigate();
	const location = useLocation();

	const { sidebarOpen } = useAppState();
	const {
		data: systemConfiguration,
		isLoading: isLoadingSystemConfiguration,
	} = useSystemConfiguration();

	useEffect(() => {
		setLoading(isLoadingSystemConfiguration);
	}, [isLoadingSystemConfiguration]);

	useEffect(() => {
		setHideNavAndSidebar(
			!loggedIn ||
				location.pathname.toLowerCase() === "/first-run" ||
				location.pathname.toLowerCase() === "/login"
		);
	}, [loggedIn, location]);

	useEffect(() => {
		const firstRun = systemConfiguration?.firstRun;

		if (loading) {
			return;
		}
		if (firstRun && location.pathname.toLowerCase() !== "/first-run") {
			navigate("/first-run");
		}
		if (
			!loggedIn &&
			!firstRun &&
			location.pathname.toLowerCase() !== "/login"
		) {
			navigate("/login");
		}
	}, [location, navigate, systemConfiguration, loading, loggedIn]);

	return (
		<div className={styles.application}>
			{loading ? (
				<LoadingIndicator />
			) : (
				<>
					{hideNavAndSidebar ? null : <Navbar />}
					<div className={styles.body}>
						<>
							{hideNavAndSidebar ? null : (
								<div
									className={cn(
										styles.sidebar,
										sidebarOpen
											? styles.open
											: styles.closed
									)}>
									<Sidebar />
								</div>
							)}
							<div className={styles.main}>
								<div
									className={styles.pageContent}
									id="main-page-content">
									<Routes>
										<Route path="/" element={<Home />} />
										<Route
											path="/login"
											element={<Login />}
										/>
										<Route
											path="/first-run"
											element={<FirstRun />}
										/>
										<Route
											path="/library/:libraryId"
											element={<Library />}
										/>
										<Route
											path="/library/:libraryId/series/:seriesId"
											element={<Series />}
										/>
										<Route
											path="/library/:libraryId/series/:seriesId/book/:bookId"
											element={<Book />}
										/>
									</Routes>
								</div>
							</div>
						</>
					</div>
				</>
			)}
		</div>
	);
};
