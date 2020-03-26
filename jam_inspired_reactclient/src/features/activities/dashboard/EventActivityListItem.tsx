import React from 'react';
import { Item, Button, Segment, Icon, Label } from 'semantic-ui-react';
import { Link } from 'react-router-dom';
import { IEventActivity } from '../../../app/models/event-activity';
import { observer } from 'mobx-react-lite';
import { format } from 'date-fns';
import EventActivityListItemAttendees from './EventActivityListItemAttendees';

const EventActivityListItem: React.FC<{ event: IEventActivity }> = ({ event }) => {
  const host = event.attendees.filter(x => x.isHost)[0]
  return (
    <Segment.Group>
      <Segment>
        <Item.Group>
          <Item>
            <Item.Image size='tiny' circular src={host.image || '/assets/user.png'} />
            <Item.Content>
              <Item.Header as={Link} to={`/events/${event.id}`}>{event.title}</Item.Header>
              <Item.Description>
                Hosted by {host.displayName}
            </Item.Description>
              {event.isHost && 
                <Item.Description>
                  <Label color='green' content='You are the host of this event' />
                </Item.Description>}
              {event.isGoing && !event.isHost &&
                <Item.Description>
                  <Label color='yellow' content='You are attending this event' />
                </Item.Description>}
            </Item.Content>
          </Item>
        </Item.Group>
      </Segment>
      <Segment>
        <Icon name='clock' /> {format(event.date, 'h:mm a')}
        <Icon name='marker' /> {event.venue}, {event.city}
      </Segment>
      <Segment secondary>
        <EventActivityListItemAttendees attendees={event.attendees} />
      </Segment>
      <Segment clearing>
        <span>{event.description}</span>
        <Button
          as={Link} to={`/events/${event.id}`}
          floated='right'
          content='View'
          color='green'
        />
      </Segment>
    </Segment.Group>

  )
}

export default observer(EventActivityListItem);