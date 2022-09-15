import {
	faBook,
	faBookOpen,
	faChartPie,
	faDesktop,
	faFileImport,
	faHouseChimney
} from "@fortawesome/free-solid-svg-icons";
import { LibraryModal } from "components/libraryModal";
import { useAppState } from "context/AppStateContext";
import { Library } from "models/api-models";
import { useEffect, useState } from "react";
import { Link, useLocation } from "react-router-dom";
import { useLibraries } from "services/library";
import styles from "./Sidebar.module.scss";
import { SidebarMenuItem } from "./SidebarMenuItem";
import { SidebarMenuItemDropdown } from "./SidebarMenuItemDropdown";
import { SidebarSubmenuItem } from "./SidebarSubmenuItem";

export const Sidebar = () => {
  const [showLibraryModal, setShowLibraryModal] = useState(false);
  const [activeMenuItem, setActiveMenuItem] = useState("");

  const { data: libraries } = useLibraries();
  const { pathname } = useLocation();
  const { sidebarOpen } = useAppState();

  useEffect(() => {
    var libraryId = libraries?.find((l) => pathname.includes(l.id!))?.id;
    if (!!libraryId) {
      setActiveMenuItem(libraryId);
      return;
    }
    if (pathname === "/") {
      setActiveMenuItem("home");
    }
  }, [pathname, libraries]);

  return (
    <>
      <div className={styles.sidebar}>
        <ul>
          <li key="home">
            <Link to={"/"} style={{ textDecoration: "none" }}>
              <SidebarMenuItem
                name="Home"
                collapsed={!sidebarOpen}
                id="home"
                icon={faHouseChimney}
                setActive={setActiveMenuItem}
                activeItem={activeMenuItem}
              />
            </Link>
          </li>
          {libraries &&
            libraries?.map((library: Library) => {
              return (
                <li key={library.id}>
                  <Link
                    to={`/library/${library.id}`}
                    style={{ textDecoration: "none" }}>
                    <SidebarMenuItem
                      name={library.name || ""}
                      collapsed={!sidebarOpen}
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
          <li key="dashboard">
            <Link to={"/dashboard"} style={{ textDecoration: "none" }}>
              <SidebarMenuItem
                name="Dashboard"
                collapsed={!sidebarOpen}
                id="dashboard"
                icon={faChartPie}
                setActive={setActiveMenuItem}
                activeItem={activeMenuItem}
              />
            </Link>
          </li>{" "}
          <li key="import">
            <Link to={"/import"} style={{ textDecoration: "none" }}>
              <SidebarMenuItem
                name="Import"
                collapsed={!sidebarOpen}
                id="import"
                icon={faFileImport}
                setActive={setActiveMenuItem}
                activeItem={activeMenuItem}
              />
            </Link>
          </li>
          <li>
            <SidebarMenuItemDropdown
              id="application"
              name="Application"
              collapsed={!sidebarOpen}
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
