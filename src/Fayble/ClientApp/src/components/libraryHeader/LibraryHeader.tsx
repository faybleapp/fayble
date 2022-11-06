import {
  faEdit,
  faList,
  faSearchPlus,
  faTh
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import cn from "classnames";
import { Breadcrumb } from "components/breadcrumb";
import { BreadcrumbItem, ViewType } from "models/ui-models";
import { NavDropdown, OverlayTrigger, Tooltip } from "react-bootstrap";
import { useRunBackgroundTask, useScanlibrary } from "services";
import styles from "./LibraryHeader.module.scss";

interface LibraryHeaderProps {
  navItems: BreadcrumbItem[];
  libraryId: string;
  libraryView: ViewType;
  changeView: (view: ViewType) => void;
  openEditModal: () => void;
}

export const LibraryHeader = ({
  navItems,
  openEditModal,
  changeView,
  libraryView,
  libraryId,
}: LibraryHeaderProps) => {
  const runBackgroundTask = useRunBackgroundTask();
  const scanLibrary = useScanlibrary(libraryId);

  const scan = () => {
    // if (scanTask.running && scanTask.libraryId === props.libraryId) {
    // 	toast.warn("Library scan already running");
    // 	return;
    // }
    // runTask(props.libraryId, TaskType.LibraryScan);
    scanLibrary.mutate(null);
    // runBackgroundTask.mutate({
    //   itemId: libraryId,
    //   taskType: BackgroundTaskType[BackgroundTaskType.LibraryScan],
    // });
  };

  return (
    <div className={cn(styles.libraryHeader)}>
      <Breadcrumb items={navItems} />
      <div className={styles.toolbar}>
        <div className={cn(styles.libraryHeaderButton)}>
          <OverlayTrigger
            overlay={<Tooltip id="h2i">Edit</Tooltip>}
            placement="top">
            <div>
              <FontAwesomeIcon
                icon={faEdit}
                className={styles.libraryHeaderIcon}
                onClick={openEditModal}
              />
            </div>
          </OverlayTrigger>
        </div>
        <div className={styles.libraryHeaderButton}>
          <OverlayTrigger
            overlay={<Tooltip id="h3i">Scan Library</Tooltip>}
            placement="top">
            <div>
              <FontAwesomeIcon
                className={styles.libraryHeaderIcon}
                icon={faSearchPlus}
                onClick={scan}
              />
            </div>
          </OverlayTrigger>
        </div>
        <div className={styles.libraryHeaderMenu}>
          <OverlayTrigger
            overlay={<Tooltip id="hi">Change View</Tooltip>}
            placement="top">
            <div>
              <NavDropdown
                id="library-view-toggle"
                title={
                  <FontAwesomeIcon
                    className={styles.libraryHeaderIcon}
                    icon={libraryView === ViewType.CoverGrid ? faTh : faList}
                  />
                }>
                <NavDropdown.Item
                  onClick={() => changeView(ViewType.CoverGrid)}>
                  Cover View
                </NavDropdown.Item>
                <NavDropdown.Item onClick={() => changeView(ViewType.List)}>
                  List View
                </NavDropdown.Item>
              </NavDropdown>
            </div>
          </OverlayTrigger>
        </div>
      </div>
    </div>
  );
};
